using BankIssueRegApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Contracts
{
    public interface IMailService
    {

        void SendMail(EmailModelDto emailDataDto);

    }
}
