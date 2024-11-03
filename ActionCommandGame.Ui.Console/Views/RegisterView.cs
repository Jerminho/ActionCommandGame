using ActionCommandGame.DTO.Requests;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class RegisterView : IView
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterView(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n\t\tWelcome to the Game!\n");
            Console.ResetColor();

            Console.WriteLine("\t\t========================");
            Console.WriteLine("\t\t  Register Account      ");
            Console.WriteLine("\t\t========================\n");

            // Collect user input
            var email = PromptForInput("Email: ");
            var password = PromptForInput("Password: ", true);

            // Attempt registration
            var result = await _authenticationService.Register(new UserRegisterRequestDto { Email = email, Password = password });

            // Provide feedback
            if (!string.IsNullOrEmpty(result.Token))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\t\tRegistration successful! Token: " + result.Token);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tRegistration failed. Please try again.");
            }

            // Reset console color
            Console.ResetColor();
            Console.WriteLine("\n\tPress any key to continue...");
            Console.ReadKey();
        }

        private string PromptForInput(string prompt, bool maskInput = false)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(prompt);
            Console.ResetColor();
            if (maskInput)
            {
                // Mask input for passwords
                StringBuilder input = new StringBuilder();
                ConsoleKeyInfo key;

                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input.Length--;
                        Console.Write("\b \b");
                    }
                    else if (key.KeyChar != 0)
                    {
                        input.Append(key.KeyChar);
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
                return input.ToString();
            }
            else
            {
                return Console.ReadLine()!;
            }
        }
    }
}
