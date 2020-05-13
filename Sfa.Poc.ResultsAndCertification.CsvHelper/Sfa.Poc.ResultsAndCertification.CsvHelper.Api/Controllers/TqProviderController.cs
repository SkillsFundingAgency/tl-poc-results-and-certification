using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Filters;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Infrastructure;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TqProviderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProviderService _providerService;
        private readonly ITqAwardingOrganisationService _tqAwardingOrganisationService;
        private readonly ITqProviderService _tqProviderService;        
        private readonly ITqRouteService _tqRouteService;
        private readonly ITqPathwayService _tqPathwayService;
        private readonly ITqSpecialismService _tqSpecialismService;

        public TqProviderController(IMapper mapper,
            IProviderService providerService,
            ITqAwardingOrganisationService tqAwardingOrganisationService,
            ITqProviderService tqProviderService,
            ITqRouteService tqRouteService,
            ITqPathwayService tqPathwayService,
            ITqSpecialismService tqSpecialismService)
        {
            _mapper = mapper;
            _providerService = providerService;
            _tqAwardingOrganisationService = tqAwardingOrganisationService;
            _tqProviderService = tqProviderService;
            _tqRouteService = tqRouteService;
            _tqPathwayService = tqPathwayService;
            _tqSpecialismService = tqSpecialismService;
        }

        [HttpPost]
        [Route("register-tqprovider", Name = "RegisterTechnicalQualificationProvider")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateModel]
        public async Task<IActionResult> RegisterTechnicalQualificationProvider(RegisterTqProvider registerProvider)
        {
            var ao = await _tqAwardingOrganisationService.GetTqAwardingOrganisationDetailsByCodeAsync(registerProvider.UkAoCode);
            //if(ao == null) return BadRequest(new BadRequestResponse($"AoCode '{registerProvider.UkAoCode}' does not exist."));
            if (!ao.Success) return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, ao.Message));

            var provider = await _providerService.GetProviderDetailsByCodeAsync(registerProvider.UkProviderCode);
            if (provider == null) return BadRequest(new BadRequestResponse($"Provider Code '{registerProvider.UkProviderCode}' does not exist."));

            var tqRoute = await _tqRouteService.GetTqRouteDetailsByCodeAsync(registerProvider.TqRouteCode);
            if (tqRoute == null) return BadRequest(new BadRequestResponse($"Tq Route Code '{registerProvider.TqRouteCode}' does not exist."));

            var tqPathway = await _tqPathwayService.GetTqPathwayDetailsByCodeAsync(registerProvider.TqPathwayCode);
            if (tqPathway == null) return BadRequest(new BadRequestResponse($"Tq Pathway Code '{registerProvider.TqRouteCode}' does not exist."));

            var tqSpecialism = await _tqSpecialismService.GetTqSpecialismDetailsByCodeAsync(registerProvider.TqSpecialismCode);
            if (tqSpecialism == null) return BadRequest(new BadRequestResponse($"Tq Specialism Code '{registerProvider.TqRouteCode}' does not exist."));

            var tqProvider = new TqProviderDetails
            {
                AwardingOrganisationId = ao.Value.Id,
                ProviderId = provider.Id,
                RouteId = tqRoute.Id,
                PathwayId = tqPathway.Id,
                SpecialismId = tqSpecialism.Id
            };

            if(await _tqProviderService.CheckIfTqProviderAlreadyExistsAsync(tqProvider))
            {
                return BadRequest(new BadRequestResponse("A record was not created because a duplicate of the current record already exists."));
            }
            var tqProviderId = await _tqProviderService.CreateTqProvidersAsync(tqProvider);

            //return tqProvider != 0 ? Ok() : (ActionResult)BadRequest();
            return CreatedAtAction(nameof(GetRegisteredTqProviderInformation), new { tqProviderId }, registerProvider);
        }

        [HttpGet]
        [Route("registeredtqprovider-information/{tqProviderId}", Name = "GetRegisteredTqProviderInformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRegisteredTqProviderInformation(int tqProviderId)
        {
            var user = ControllerContext.HttpContext.User;
            var registerdTqProviderDetails = await _tqProviderService.GetRegisteredTqProviderDetailsByIdAsync(tqProviderId);
            return (registerdTqProviderDetails != null) ? Ok(registerdTqProviderDetails) : (ActionResult)NotFound();

            //return (registerdTqProviderDetails != null) ? Ok(new OkResponse<RegisteredTqProviderDetails>(registerdTqProviderDetails)) : (ActionResult)NotFound(new ApiResponse(404, $"Registered Tq Provider not found with id {tqProviderId}"));
        }

        [HttpPost]
        //[Produces("application/json", "multipart/form-data")]
        [Route("onboard-tqprovider", Name = "OnBoardTechnicalQualificationProvider")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateModel]
        public async Task<IActionResult> OnBoardTechnicalQualificationProvider(OnboardTqProvider registerProvider)
        {
            var ao = await _tqAwardingOrganisationService.GetTqAwardingOrganisationDetailsByCodeAsync(registerProvider.AoCode);
            //if(ao == null) return BadRequest(new BadRequestResponse($"AoCode '{registerProvider.UkAoCode}' does not exist."));
            if (!ao.Success) return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, ao.Message));

            foreach (var provider in registerProvider.Providers)
            {
                var providerDetails = await _providerService.GetProviderDetailsByCodeAsync(provider.UkPrnNumber);
                if (providerDetails == null) return BadRequest(new BadRequestResponse($"Provider Code '{provider.UkPrnNumber}' does not exist."));

                foreach(var route in provider.Routes)
                {
                    var tqRoute = await _tqRouteService.GetTqRouteDetailsByCodeAsync(route.RouteId);
                    if (tqRoute == null) return BadRequest(new BadRequestResponse($"Tq Route Code '{route.RouteId}' does not exist."));

                    foreach(var pathway in route.Pathways)
                    {
                        var tqPathway = await _tqPathwayService.GetTqPathwayDetailsByCodeAsync(pathway.PathwayId);
                        if (tqPathway == null) return BadRequest(new BadRequestResponse($"Tq Pathway Code '{pathway.PathwayId}' does not exist."));

                        foreach(var specialism in pathway.Specialisms)
                        {
                            var tqSpecialism = await _tqSpecialismService.GetTqSpecialismDetailsByCodeAsync(specialism.SpecialismId);
                            if (tqSpecialism == null) return BadRequest(new BadRequestResponse($"Tq Specialism Code '{specialism.SpecialismId}' does not exist."));
                        }
                    }
                }
            }

            //var tqProvider = new TqProviderDetails
            //{
            //    AwardingOrganisationId = ao.Value.Id,
            //    ProviderId = provider.Id,
            //    RouteId = tqRoute.Id,
            //    PathwayId = tqPathway.Id,
            //    SpecialismId = tqSpecialism.Id
            //};

            //if (await _tqProviderService.CheckIfTqProviderAlreadyExistsAsync(tqProvider))
            //{
            //    return BadRequest(new BadRequestResponse("A record was not created because a duplicate of the current record already exists."));
            //}
            //var tqProviderId = await _tqProviderService.CreateTqProvidersAsync(tqProvider);

            var tqProviderId = 7;
            //return tqProvider != 0 ? Ok() : (ActionResult)BadRequest();
            return CreatedAtAction(nameof(GetRegisteredTqProviderInformation), new { tqProviderId }, registerProvider);
        }


        [HttpPost]
        //[Produces("application/json", "multipart/form-data")]
        [Route("upload-resultsfile", Name = "UploadResultsFile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateModel]
        public async Task<IActionResult> UploadResultsFile([FromForm]TestFileUpload fileUpload)
        {
            var provider = await _providerService.GetProviderDetailsByCodeAsync(12345);
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