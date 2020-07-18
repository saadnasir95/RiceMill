using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Bank.Commands.UpdateBank
{

    public class UpdateBankRequestHandler : IRequestHandler<UpdateBankRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateBankRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateBankRequestModel request, CancellationToken cancellationToken)
        {
            var bank = _context.Banks.GetBy(p => p.Id == request.BankId);
            if (bank == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Bank),request.BankId);
            }

            bank.Name = request.Name;
            _context.Update(bank);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new Response()
            {
                Id = bank.Id,
                Name = bank.Name,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(bank.CreatedDate)
            });
        }

        class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CreatedDate { get; set; }
        }
        
    }

}