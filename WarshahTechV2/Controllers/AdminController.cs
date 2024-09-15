using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.SparePartsDTOs;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.MailModels;
using DL.MailSales;
using DocumentFormat.OpenXml.EMMA;
using Helper;
using HELPER;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace WarshahTechV2.Controllers
{
    [ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper _mapper;
        private readonly ISMS _SMS;
        private readonly IMailService _MailService;
        private readonly INotificationService _NotificationService;
        public AdminController(IUnitOfWork uow, INotificationService notificationService, IMapper _mapper, ISMS sMS, IMailService MailService)
        {
            this.uow = uow;
            this._mapper = _mapper;
            _SMS = sMS;
            _MailService = MailService;
            _NotificationService = notificationService;
        }



        [HttpGet, Route("mail")]
        public IActionResult mail()
        {
            var Email = "sahmed@onetelecom.me";

            var message = "مرحبا بك فى نظام ورشة تك الجديد";


            _MailService.Notification(Email, message);

            return Ok();
        }


        [HttpGet, Route("GetAllCounts")]
        public IActionResult GetAllCounts()
        {
            var warshahCount = uow.WarshahRepository.GetAll().Count();
            var UserCount = uow.UserRepository.GetAll().Count();
            var RepairOrdersCount = uow.RepairOrderRepository.GetAll().Count();
            var MotorCount = uow.MotorsRepository.GetAll().Count();
            var InvoicesCount = uow.SubscribtionInvoicerepository.GetAll().Count();
            var SalesRequestCount = uow.SalesRequestRepository.GetAll().Count();
            var Ob = new { warshahCount = warshahCount, UserCount = UserCount, RepairOrdersCount = RepairOrdersCount, MotorCount = MotorCount, InvoicesCount = InvoicesCount, SalesRequestCount = SalesRequestCount };
            return Ok(Ob);
        }
        public class WasrshahUserDTo
        {
            public User User { get; set; }
            public Warshah Warshah { get; set; }
        }
        [HttpGet, Route("GetAllWarshah")]
        public IActionResult GetAllWarshah(DateTime? from, DateTime? to)
        {
            var AllDuration = uow.DurationRepository.GetAll();
            var AllWarshah = new List<WasrshahUserDTo>();
            foreach (var item in AllDuration)
            {
                if (item.EndDate > System.DateTime.Now)
                {
                    var warsah = uow.WarshahRepository.GetById(item.WarshahId);
                    var User = uow.UserRepository.GetMany(a => a.WarshahId == item.WarshahId && a.RoleId == 1).FirstOrDefault();
                    AllWarshah.Add(new WasrshahUserDTo
                    {
                        User = User,
                        Warshah = warsah
                    });
                }

            }
            var RangedData = AllWarshah.Where(a => a.Warshah?.CreatedOn.Date >= from && a.Warshah?.CreatedOn.Date <= to).ToHashSet();
            return Ok(RangedData);
        }




        [ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
        [HttpGet, Route("GetAllWarshahTechServiceByWarshahIdInAdmin")]
        public IActionResult GetAllWarshahTechServiceByWarshahIdInAdmin(int Warshahid)
        {
            var warshahservice = uow.WarshahTechServiceRepository.GetMany(a => a.WarshahId == Warshahid).ToHashSet();
            return Ok(warshahservice); 
        }



        [ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
        [HttpGet, Route("GetAllWarshahTechServiceByTypePaymentInAdmin")]
        public IActionResult GetAllWarshahTechServiceByTypePaymentInAdmin(int paymentId , int Warshahid)
        {
            var warshahservice = uow.WarshahTechServiceRepository.GetMany(a => a.PaymentTypeInvoiceId == paymentId && a.WarshahId == Warshahid).ToHashSet();
            return Ok(warshahservice);
        }




        [HttpGet, Route("StopWarshah")]
        public IActionResult StopWarshah(int warshahid, bool Cond)
        {
            var warshah = uow.WarshahRepository.GetById(warshahid);
            warshah.IsActive = Cond;

            uow.WarshahRepository.Update(warshah);
            uow.Save();
            return Ok(warshah);
        }
        [HttpGet, Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {

            return Ok(uow.UserRepository.GetAll());
        }
        [HttpPost, Route("EditWarshah")]
        public IActionResult EditWarshah([FromForm] EditWarshahDTO warshahDTO)
        {
            if (ModelState.IsValid)
            {


                var Warshah = _mapper.Map<DL.Entities.Warshah>(warshahDTO);
                var OldLogo = uow.WarshahRepository.GetById(warshahDTO.Id);
                if (OldLogo != null)
                {
                    Warshah.WarshahLogo = OldLogo.WarshahLogo;
                    uow.WarshahRepository.Update(Warshah);
                    uow.Save();
                }
                uow.WarshahRepository.Update(Warshah);
                uow.Save();


                return Ok(Warshah);
            }
            return BadRequest(ModelState);
        }


        [HttpGet, Route("Marketing")]
        public IActionResult Marketing(string mobile, string Email, string message)
        {
            if (mobile != null)
            {
                _SMS.SendSMS(mobile, message, null, false);
            }
            if (Email != null)
            {
                _MailService.Notification(Email, message);
            }
            return Ok();
        }
        [HttpGet, Route("SendMessageToWarshahByCityId")]
        public IActionResult SendMessageToWarshahByCityId(int cityId, string Message, bool SMS, bool Email)
        {
            try
            {
                var AllWarshahByCityId = uow.WarshahRepository.GetMany(a => a.CityId == cityId).ToHashSet();
                foreach (var item in AllWarshahByCityId)
                {
                    var User = uow.UserRepository.GetMany(a => a.WarshahId == item.Id && a.RoleId == 1).FirstOrDefault();
                    if (SMS)
                    {
                        _SMS.SendSMS(User.Phone, Message, null, false);
                    }
                    if (User.Email != null && Email)
                    {
                        _MailService.Notification(User.Email, Message);
                    }
                }
                return Ok();
            }
            catch (System.Exception)
            {

                return Ok();
            }


        }
        [HttpGet, Route("SendMessageToCarOwnerByModelId")]
        public IActionResult SendMessageToCarOwnerByModelId(int ModelId, string Message, bool SMS, bool Email)
        {
            var AllMotorsByModelId = uow.MotorsRepository.GetMany(a => a.MotorModelId == ModelId).ToHashSet();
            foreach (var item in AllMotorsByModelId)
            {
                var User = uow.UserRepository.GetMany(a => a.Id == item.CarOwnerId).FirstOrDefault();
                if (SMS)
                {
                    _SMS.SendSMS(User.Phone, Message, null, false);
                }
                if (User.Email != null && Email)
                {
                    _MailService.Notification(User.Email, Message);
                }
            }
            return Ok();

        }
        [HttpPost, Route("AddSmsPackage")]
        public IActionResult AddSmsPackage(SMSPackage sMSPackage)
        {
            uow.SMSPackageRepository.Add(sMSPackage);
            uow.Save();
            return Ok(sMSPackage);
        }
        [HttpPost, Route("AddSubPackage")]
        public IActionResult AddSubPackage(Subscribtion Subscribtion)
        {
            uow.SubscribtionRepository.Add(Subscribtion);
            uow.Save();
            return Ok(Subscribtion);
        }
        [HttpPost, Route("AddCupon")]
        public IActionResult AddCupon(Cupon Cupon)
        {
            uow.CuponRepository.Add(Cupon);
            uow.Save();
            return Ok(Cupon);
        }
        [HttpGet, Route("getAllSMSPackage")]
        public IActionResult getAllSMSPackage()
        {
            return Ok(uow.SMSPackageRepository.GetAll());
        }
        [HttpGet, Route("GetAllCupons")]
        public IActionResult GetAllCupons()
        {
            return Ok(uow.CuponRepository.GetAll());
        }
        [HttpGet, Route("GetAllSubs")]
        public IActionResult GetAllSubs()
        {
            return Ok(uow.SubscribtionRepository.GetAll());
        }
        [HttpPost, Route("AddCommentToDisabled")]
        public IActionResult AddCommentToDisabled(WarshahDisableReason warshahDisableReason)
        {
            warshahDisableReason.CreatedOn = System.DateTime.Now;
            uow.WarshahDisableReasonRepository.Add(warshahDisableReason);

            uow.Save();
            return Ok(warshahDisableReason);
        }
        [HttpGet, Route("GetAllCommentReason")]
        public IActionResult GetAllCommentReason()
        {
            return Ok(uow.WarshahDisableReasonRepository.GetAll().ToHashSet());
        }
        [HttpPost, Route("editSmsPackage")]
        public IActionResult editSmsPackage(SMSPackage sMSPackage)
        {
            uow.SMSPackageRepository.Update(sMSPackage);
            uow.Save();
            return Ok(sMSPackage);
        }
        [HttpPost, Route("EditGlobalSetting")]
        public IActionResult EditGlobalSetting(GlobalSetting GlobalSetting)
        {
            uow.GlobalSettingRepository.Update(GlobalSetting);
            uow.Save();


            var AllUsers = uow.UserRepository.GetAll().ToHashSet();
            foreach (var item in AllUsers)
            {
                _NotificationService.SetNotification(item.Id, "تم تعديل سياسة الاستخدام الرجاء الاطلاع عليها");
            }
            return Ok(GlobalSetting);
        }
        [HttpPost, Route("AddGlobalSetting")]
        public IActionResult AddGlobalSetting(GlobalSetting GlobalSetting)
        {
            uow.GlobalSettingRepository.Add(GlobalSetting);
            uow.Save();
            return Ok(GlobalSetting);
        }
        [HttpGet, Route("GetGlobalSetting")]
        public IActionResult GetGlobalSetting()
        {
            return Ok(uow.GlobalSettingRepository.GetAll());
        }
        [HttpPost, Route("editsubPackage")]
        public IActionResult editsubPackage(Subscribtion Subscribtion)
        {
            uow.SubscribtionRepository.Update(Subscribtion);
            uow.Save();
            return Ok(Subscribtion);
        }
        [HttpPost, Route("editCupons")]
        public IActionResult editSmsPackage(Cupon Cupons)
        {
            uow.CuponRepository.Update(Cupons);
            uow.Save();
            return Ok(Cupons);
        }

        [HttpGet, Route("GetWarshahInvoiceByWarshahId")]
        public IActionResult GetWarshahInvoiceByWarshahId(int WarshahId)
        {
            var warshah = uow.WarshahRepository.GetById(WarshahId);
            var City = uow.CityRepository.GetById(warshah.CityId);
            var Country = uow.CountryRepository.GetById(warshah.CityId);
            var Region = uow.RegionRepository.GetById(warshah.CityId);
            var sub = uow.SubscribtionInvoicerepository.GetMany(a => a.WarshahId == WarshahId).FirstOrDefault();
            return Ok(new { sub = sub, warshah = warshah, City = City, Country = Country, Region = Region });
        }
        [HttpGet, Route("GetAllExpiredWarshah")]
        public IActionResult GetAllExpiredWarshah()
        {
            var AllDuration = uow.DurationRepository.GetAll();
            var AllWarshah = new List<Warshah>();
            foreach (var item in AllDuration)
            {
                if (item.EndDate < System.DateTime.Now)
                {
                    var warsah = uow.WarshahRepository.GetById(item.WarshahId);
                    AllWarshah.Add(warsah);
                }

            }
            return Ok(AllWarshah);
        }

        public class SendContactusFormContent
        {

            public string FullName { get; set; }

            public string Phone { get; set; }
            public string Email { get; set; }

            public int RequestTypeId { get; set; }
            public int WorkTypeId { get; set; }

            public string Message { get; set; }






        }

        [HttpPost, Route("SendContactusFormInfo")]
        public IActionResult SendContactusFormInfo(SendContactusFormContent SendContactusFormContent)
        {

            if (SendContactusFormContent.RequestTypeId == 0)
                return BadRequest("Invalid ,Request Type = 0");


            if (SendContactusFormContent.WorkTypeId == 0)
                return BadRequest("Invalid, worktype = ");

            if (SendContactusFormContent.Email == null)
                return BadRequest("sorry , Email is Empty");
            if (SendContactusFormContent.Phone == null)
                return BadRequest("sorry , Phone is Empty");
            var RequestType = uow.RequestTypeRepository.GetById(SendContactusFormContent.RequestTypeId);
            var WorkType = uow.WorkTypeRepository.GetById(SendContactusFormContent.WorkTypeId);

            try
            {
                MailSender sender = new MailSender();
                string FilePath = Directory.GetCurrentDirectory() + @"\wwwroot\EmailTemps\ContactUsformtemplate.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText
                .Replace("{FullName}", SendContactusFormContent.FullName)
                              .Replace("{Email}", SendContactusFormContent.Email)
                                 .Replace("{Phone}", SendContactusFormContent.Phone)
                                    .Replace("{RequestTypeId}", RequestType.Namear)
                   .Replace("{WorkTypeId}", WorkType.NameAr)
                                                   .Replace("{Message}", SendContactusFormContent.Message);


                var email = "info@warshahtech.sa";


                bool isSent = sender.SendMailcontactusInfo(email, SendContactusFormContent.Email , MailText, true);




                return Ok();
            }
            catch (Exception)
            {

                throw;
            }


        }





    }
}
