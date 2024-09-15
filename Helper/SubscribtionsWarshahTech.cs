using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.SubscribtionDTOs;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public interface ISubscribtionsWarshahTech
    {
        SubscribtionInvoice CreateSubscribtionInvoice(SubscribtionInvoiceDTO subscribtionInvoiceDTO);

    }


    public class SubscribtionsWarshahTech : ISubscribtionsWarshahTech
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SubscribtionsWarshahTech(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }



        public SubscribtionInvoice CreateSubscribtionInvoice(SubscribtionInvoiceDTO subscribtionInvoiceDTO)
        {
            //var InvoiceSerialDTO = new InvoiceSerialDTO();
            // Get last invoice number for each warshash
            try
            {
                var invoicenumber = _uow.SubscribtionInvoicerepository.GetMany(i => i.WarshahId == subscribtionInvoiceDTO.WarshahId).OrderByDescending(t => t.InvoiceNumber).FirstOrDefault();
                if (invoicenumber == null)
                {
                    subscribtionInvoiceDTO.InvoiceNumber = 1;
                }
                else
                {
                    int lastnumber = invoicenumber.InvoiceNumber;
                    subscribtionInvoiceDTO.InvoiceNumber = lastnumber + 1;
                }
                subscribtionInvoiceDTO.InvoiceSerial = "IN-" + "WarshahTech" + "-" + subscribtionInvoiceDTO.InvoiceNumber;

                var invoice = _mapper.Map<DL.Entities.SubscribtionInvoice>(subscribtionInvoiceDTO);

                var v = 15;
                decimal Vat = (((decimal)v) / (100));
                decimal V = (decimal)System.Convert.ToDouble(1.15);

                invoice.SubscribtionWithoutVat = invoice.TotalSubscribtion / V;
                invoice.SubscribtionVat = invoice.TotalSubscribtion - invoice.SubscribtionWithoutVat;


                _uow.SubscribtionInvoicerepository.Add(invoice);
                _uow.Save();
                return invoice;

            }
            catch (Exception)
            {
                var invoice = new SubscribtionInvoice();    

                return invoice;
            }
            
        }


    }


    }
