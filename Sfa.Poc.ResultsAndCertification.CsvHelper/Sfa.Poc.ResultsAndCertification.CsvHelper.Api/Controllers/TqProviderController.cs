using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TqProviderController : ControllerBase
    {
        private readonly IMapper _mapper;

        public TqProviderController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        //[Produces("application/json", "multipart/form-data")]
        [Route("upload-resultsfile", Name = "UploadResultsFile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateModel]
        public IActionResult UploadResultsFile([FromForm]TestFileUpload fileUpload)
        {
            var fi = fileUpload;
            return Ok();
        }
    }

    public class TestFileUpload
    {
        public int Id { get; set; }
        
        public IFormFile File { get; set; }
    }
}