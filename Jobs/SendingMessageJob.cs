using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Controllers;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Extensions;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Razor.Parser.SyntaxTree;

namespace Plan_current_repairs.Jobs
{
    public class SendingMessageJob : IJob
    {
        public readonly IEmployee employee;
        public readonly IEmailService emailService;
        public readonly Settings settings;
        public readonly ILogger<SendingMessageJob> logger;
        public SendingMessageJob(IEmployee _employee, IEmailService _emailService, IOptionsMonitor<Settings> _settings, ILogger<SendingMessageJob> _logger)
        {
            employee = _employee;
            emailService = _emailService;
            settings = _settings.CurrentValue;
            logger = _logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
           logger.LogInformation("{currentDate}: начало работы по отправки сообщений", DateTime.Now);

           var currentDate = DateTime.Now;
           const string url = "http://stg-usv-web03/Plan_current_repairs.com/";
           const string subject = "План-отчет по текущему ремонту";
           List<string> ListEmailForHeadOfDepartment=employee.GetListEmailForHeadOfDepartment();

            string reportEmail = "email@domen.ru";
            string EngineerOfPTOEmail = employee.GetEmailForFullName(settings.EngineerOfPTO_Name) != null ? employee.GetEmailForFullName(settings.EngineerOfPTO_Name) : "email@domen.ru";

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 1, 10).ToShortDateString()) == 0)
            {
                foreach(var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо заполнить план на "+ currentDate.Year+" год. "+ url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 1, 20).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение плана на " + currentDate.Year + " год и сдать в ПТО. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 1, 25).ToShortDateString()) == 0)
            {              
                await emailService.SendEmailAsync(EngineerOfPTOEmail, subject, "Напоминание: наступило время проверки планов на " + currentDate.Year + " год. " + url);
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 2, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за январь " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 3, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за февраль " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 4, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за март " + currentDate.Year + " года и сдать отчет за I квартал в ПТО. " + url);
                }
                await emailService.SendEmailAsync(EngineerOfPTOEmail, subject, "Напоминание: наступило время проверки отчетов за I квартал " + currentDate.Year + " года. " + url);
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 5, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за апрель " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 6, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за май " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 7, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за июнь " + currentDate.Year + " года и сдать отчет за II квартал в ПТО. " + url);
                }
                await emailService.SendEmailAsync(EngineerOfPTOEmail, subject, "Напоминание: наступило время проверки отчетов за II квартал " + currentDate.Year + " года. " + url);
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 8, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за июль " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 9, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за август " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 10, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за сентябрь " + currentDate.Year + " года и сдать отчет за III квартал в ПТО. " + url);
                }
                await emailService.SendEmailAsync(EngineerOfPTOEmail, subject, "Напоминание: наступило время проверки отчетов за III квартал " + currentDate.Year + " года. " + url);
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 11, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за октябрь " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 12, 10).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за ноябрь " + currentDate.Year + " года. " + url);
                }
                goto endMethod;
            }

            if (currentDate.ToShortDateString().CompareTo(new DateTime(currentDate.Year, 12, 25).ToShortDateString()) == 0)
            {
                foreach (var email in ListEmailForHeadOfDepartment)
                {
                    await emailService.SendEmailAsync(email, subject, "Напоминание: необходимо завершить заполнение отчета за декабрь " + currentDate.Year + " года и сдать отчет за IV квартал в ПТО. " + url);
                }
                await emailService.SendEmailAsync(EngineerOfPTOEmail, subject, "Напоминание: наступило время проверки отчетов за IV квартал " + currentDate.Year + " года." +
                    " Согласование сводного отчета за " + currentDate.Year +" год. " + url);
                goto endMethod;
            }
            await emailService.SendEmailAsync(reportEmail, subject, currentDate + " : test system... " + url);
            return;
            endMethod: await emailService.SendEmailAsync(reportEmail, subject, currentDate + " : сообщения были разосланы. " + url);
        }
    }
}




