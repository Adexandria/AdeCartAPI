using AutoMapper;
using MoviesAPI.UserModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AdeCartAPI.Model;
using AdeCartAPI.UserModel;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace AdeCartAPI.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.OK, "Returns if sucessful")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Returns if not found")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Returns no content")]

    [ApiController]
    [Route("api/User")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        readonly UserManager<User> user;
        private readonly SignInManager<User> login;
        private readonly IPasswordHasher<User> passwordHasher;

        readonly IMapper mapper;

        public UserController(UserManager<User> user, IMapper mapper, SignInManager<User> login, IPasswordHasher<User> passwordHasher)
        {
            this.user = user;
            this.mapper = mapper;
            this.login = login;
            this.passwordHasher = passwordHasher;
        }
        ///<param name="newuser">
        ///an object to sign up a user
        ///</param>
        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        //The Function creates a new user
        //using user manager library
        //then logins in the newly created user
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp(SignUp newuser)
        {
            var signup = mapper.Map<User>(newuser);
            if (newuser.Password.Equals(newuser.RetypePassword))
            {
                IdentityResult identity = await user.CreateAsync(signup, signup.PasswordHash);

                if (identity.Succeeded)
                {
                    await user.AddClaimAsync(signup, new Claim(ClaimTypes.Role, "User"));
                    await login.SignInAsync(signup, false);
                    var token = await EmailConfirmationToken(signup);
                    return this.StatusCode(StatusCodes.Status201Created, $"Welcome,{signup.UserName} use this {token} verify email");
                }
                else
                {
                    return BadRequest("The username exists or check password requirements");
                }
            }
            return this.StatusCode(StatusCodes.Status400BadRequest, "Password not equal,retype password");
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        ///<param name="token">
        ///a token to confirm user's email
        ///</param>
        /// <summary>
        ///email confrimation
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{username}/emailconfirmation")]
        public async Task<ActionResult> VerifyEmailToken(Token token,string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var result = await user.ConfirmEmailAsync(currentUser, token.GeneratedToken);
            if (result.Succeeded) 
            {
                return this.StatusCode(StatusCodes.Status200OK, $"Welcome,{currentUser.UserName} Email has been verified");
            }
            else
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, "Invalid Token");
            }
        }
        ///<param name="model">
        ///an object to login a user
        ///</param>
        /// <summary>
        ///Login a  user
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        //This function sign in existing user
        //by using the signin manager library.
        [HttpPost("login")]
        public async Task<ActionResult> Login(Login model)
        {
            var logindetails = mapper.Map<User>(model);
            var currentUser = await user.FindByNameAsync(logindetails.UserName);
            if (currentUser == null) return NotFound("Username doesn't exist");
            //This verfies the user password by using IPasswordHasher interface
            var passwordVerifyResult = passwordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, model.Password);
            if (passwordVerifyResult.ToString() == "Success")
            {
                await login.SignInAsync(currentUser, false);
                await login.CreateUserPrincipalAsync(currentUser);

                return this.StatusCode(StatusCodes.Status200OK, $"Welcome,{currentUser.UserName} Shop Shop Shop");
            }

            return BadRequest("password is not correct");
        }

        ///<param name="username">
        ///\a user's username
        ///</param>
        /// <summary>
        ///reset password
        /// </summary>
        /// 
        /// <returns>A string status</returns>

        //To generate the token to reset password
        [HttpGet("{username}/password/reset")]
        public async Task<ActionResult> ResetPassword(string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var passwordResetToken = await user.GeneratePasswordResetTokenAsync(currentUser);
            return Ok($"Reset Password Token {passwordResetToken}");
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        ///<param name="resetPassword">
        ///an object to reset a password
        ///</param>
        /// <summary>
        ///verify token/reset password confirmation
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        //To verify the Password reset token
        //which gives access to change the user's password
        [HttpPost("{username}/password/verifytoken")]
        public async Task<ActionResult> VerifyPasswordToken(ResetPassword resetPassword,string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var isVerify = passwordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, resetPassword.NewPassword);
            if (isVerify.ToString() == "Success") return BadRequest("Old password can't be new password");
            var isVerifyResult = await user.ResetPasswordAsync(currentUser, resetPassword.Token, resetPassword.NewPassword);
            if (isVerifyResult.Succeeded)
            {
                return Ok("Password changed");
            }
            else
            {
                return BadRequest(isVerifyResult.Errors);
            }
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        ///<param name="phoneNumber">
        ///an object used to add user's phonenumber
        ///</param>
        /// <summary>
        ///Add phonenumber
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{username}/phonenumber/add")]
        public async Task<ActionResult<User>> AddPhoneNumber(UserNumber phoneNumber,string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var result = await user.SetPhoneNumberAsync(currentUser, phoneNumber.PhoneNumber);
            if (result.Succeeded) 
            {
                return this.StatusCode(StatusCodes.Status200OK, "Successfully Changed");
            }
            else 
            {
                return BadRequest(result.Errors);
            }
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        ///<param name="phoneNumber">
        ///an object used to change user's phonenumber
        ///</param>
        /// <summary>
        /// change phonenumber
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet("{username}/phonenumber/change")]
        public async Task<ActionResult<User>> ChangePhonenumber(string username,UserNumber phoneNumber) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var token = await user.GenerateChangePhoneNumberTokenAsync(currentUser, phoneNumber.PhoneNumber);
            var result = await user.ChangePhoneNumberAsync(currentUser, phoneNumber.PhoneNumber, token);
            if (result.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, "Successfully Changed");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        ///<param name="name">
        ///an object used to change user's username
        ///</param>
        /// <summary>
        /// change username
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{username}/change")]
        public async Task<ActionResult<User>> ChangeUsername(UserName name,string username) 
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound("username doesn't exist");
            var result = await user.SetUserNameAsync(currentUser,name.Username);
            if (result.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, "Successfully Changed");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        ///<param name="username">
        ///\a user's username
        ///</param>
        /// <summary>
        /// sign out user
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        //To sign out a user
        [HttpPost("{username}/signout")]
        public async Task<ActionResult> Signout(string username)
        {
            var currentUser = await GetUser(username);
            if (currentUser == null) return NotFound ("username doesn't exist");
            await login.SignOutAsync();
            return Ok();
        }

        [NonAction]
        private async Task<string> EmailConfirmationToken(User newUser) 
        {
          return await user.GenerateEmailConfirmationTokenAsync(newUser);
        }

        [NonAction]
        private async Task<User> GetUser(string userName) 
        {
           return await user.FindByNameAsync(userName);
        }
      
    }
}
