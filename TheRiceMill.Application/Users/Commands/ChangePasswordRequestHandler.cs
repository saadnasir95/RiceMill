using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Users.Models;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Users.Commands
{
    public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequestModel, ResponseViewModel>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordRequestHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ResponseViewModel> Handle(ChangePasswordRequestModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(p => new ValidationFailure("",p.Description)).ToList());
            }
            return new ResponseViewModel().CreateOk();
        }
    }
}