using System;
using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankTransactions.Commands.UpdateBankTransaction
{

    public class UpdateBankTransactionRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public TransactionType TransactionType { get; set; }
        public double TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int BankAccountId { get; set; }
        public string ChequeNumber { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}