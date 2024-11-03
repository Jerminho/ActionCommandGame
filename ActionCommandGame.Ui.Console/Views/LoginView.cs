using ActionCommandGame.DTO.Requests;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;

public class LoginView : IView
{
    private readonly IAuthenticationService _authenticationService;

    public LoginView(IAuthenticationService authenticationService)
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
        Console.WriteLine("\t\t     User Login        ");
        Console.WriteLine("\t\t========================\n");

        var request = new UserSignInRequestDto
        {
            Email = PromptForEmail(),
            Password = PromptForPassword()
        };

        var result = await _authenticationService.SignIn(request);

        if (result != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n\t\tLogin successful! Token: {result.Token}\n");
            Console.ResetColor();
            // Navigate to the next view
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\t\tInvalid login attempt.\n");
            Console.ResetColor();
        }

        // Wait for user input before clearing
        Console.WriteLine("\n\tPress any key to continue...");
        Console.ReadKey();
    }

    private string PromptForEmail()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\tEmail: ");
        Console.ResetColor();
        return Console.ReadLine()!;
    }

    private string PromptForPassword()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\tPassword: ");
        Console.ResetColor();
        return Console.ReadLine()!;
    }
}