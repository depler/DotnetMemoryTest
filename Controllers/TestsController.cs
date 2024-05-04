using Microsoft.AspNetCore.Mvc;

namespace DotnetTest3.Controllers;

[Route("/")]
[ApiController]
public class TestsController : ControllerBase
{
    private const int LengthMin = 200_000_000;

    [HttpGet(nameof(Download))]
    public ActionResult<byte[]> Download()
    {
        var length = Random.Shared.Next(LengthMin, LengthMin + 1);
        var data = new byte[length];

        return data;
    }
}