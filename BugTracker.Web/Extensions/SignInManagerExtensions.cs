﻿using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BugTracker.Web.Extensions
{
    public static class SignInManagerExtensions
    {
        public static async Task<SignInResult> PasswordSignInByEmailAsync<TUser>(
            this SignInManager<TUser> signInManager,
            string email,
            string password,
            bool isPersistent,
            bool lockoutOnFailure = false
        ) where TUser : class
        {
            var user = await signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(
                user, password, isPersistent, lockoutOnFailure
            );
        }
    }
}
