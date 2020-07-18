using System;
using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankTransactions.Commands.CreateBankTransaction
{

    public class CreateBankTransactionRequestModel : IRequest<ResponseViewModel>
    {
        public int CompanyId { get; set; }
        public TransactionType TransactionType { get; set; }
        public PaymentType PaymentType { get; set; }
        public double TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int BankAccountId { get; set; }
        public string ChequeNumber { get; set; }
    }
}