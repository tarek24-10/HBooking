using HBook.Application.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
