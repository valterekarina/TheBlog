using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class RestorePasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenerateRandomPassword _generateRandomPassword;

        public RestorePasswordController(IUserRepository userRepository, IGenerateRandomPassword generateRandomPassword)
        {
            _userRepository = userRepository;
            _generateRandomPassword = generateRandomPassword;
        }

        [HttpPut("restore-password")]
        public IActionResult RestorePassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(model.Email);

                if (user != null)
                {
                    //string newPassword = "test1"; 
                    string newPassword = _generateRandomPassword.RandomPasswordGenerator();
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    _userRepository.Update(user);

                    SendEmail(user, newPassword);

                    return Ok(user);
                }
                else
                {
                    return NotFound("user not found");
                }

            }
            else
            {
                return BadRequest("Modelstate invalid");
            }

        }

        private IActionResult SendEmail(User user, string password)
        {
            string emailBody = "<div>Your new password is: <strong>" + password + "</strong></div>" +
                "<div>Please use this password to log in the Blog portal.</div>" +
                "<div>If you didn't request password restoring, ignore this email.</div>";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("karina.bootcamp2023@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "The Blog password reset";
            email.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("karina.bootcamp2023@gmail.com", "qxrzwrkarpaoikrz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok("success");
        }
    }
}
