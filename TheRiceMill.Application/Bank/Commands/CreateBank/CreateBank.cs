using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Bank.Commands.CreateBank
{
    public class CreateBankRequestHandler : IRequestHandler<CreateBankRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateBankRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateBankRequestModel request, CancellationToken cancellationToken)
        {
            var bank = new Domain.Entities.Bank()
            {
                Name = request.Name
            };
            _context.Add(bank);
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