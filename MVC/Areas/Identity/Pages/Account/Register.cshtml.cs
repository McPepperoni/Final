// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.DTOs;
using Persistence.Entities;
using Persistence.Managers;

namespace MVC.Areas.Identity.Pages.Account
{
    public class RegisterModel : BasePageModel
    {

        private readonly ILogger<RegisterModel> _logger;
        private readonly IMapper _mapper;

        public RegisterModel(
            ILogger<RegisterModel> logger,
            IMapper mapper,
            IHttpClientFactory factory,
            IHttpContextAccessor accessor
        ) : base(factory, accessor)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public CreateUserDTO User { get; set; }
        }


        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/Identity/Account/Login";
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= "/Identity/User/Login";

            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync("User", Input.User);

                if (response.IsSuccessStatusCode)
                {
                    return Redirect(ReturnUrl);
                }
                ModelState.AddModelError("Input.Email", (await response.Content.ReadFromJsonAsync<ErrorDTO>()).Message);
            }

            return Page();
        }
    }
}
