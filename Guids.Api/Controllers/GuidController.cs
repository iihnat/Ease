using Microsoft.AspNetCore.Mvc;
using Guids.Api.Models.Dtos;
using Guids.Api.Managers;

namespace Guids.Api.Controllers
{
    
    [Route("api/v1/guid")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class GuidController : ControllerBase
    {
        private readonly IGuidManager _guidManager;
        public GuidController(IGuidManager guidManager)
        {
           _guidManager = guidManager;
        }
        
        [HttpPost("")]
        [ProducesResponseType(typeof(GuidInfo), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateGuid([FromBody]CreateGuidRq request)
        {
            var result = await _guidManager.CreateGuid(request);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok(result.Value);
        }
       
        [HttpGet("{guid:guid}")]
        [ProducesResponseType(typeof(GuidInfo), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMetadata(string guid)
        {
            var result = await _guidManager.GetById(guid);
            
            if (result.Error != null)
            {
                if (result.Error.ErrorCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.Error.Message);
                }
                return BadRequest(result.Error.Message);
            }
            
            return Ok(result.Value);
        }

        [HttpPut("{guid:guid}")]
        [ProducesResponseType(typeof(GuidInfo), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMetadata([FromBody]UpdateGuidMetadataRq request, string guid)
        {
            var result = await _guidManager.UpdateGuidMetadata(guid,request);
            
            if (result.Error != null)
            {
                if (result.Error.ErrorCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.Error.Message);
                }
                return BadRequest(result.Error.Message);
            }
            
            return Ok(result.Value);
        }

        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> DeleteGuid(string guid)
        {
            var result = await _guidManager.Delete(guid);
            if (result.Error != null)
            {
                if (result.Error.ErrorCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(result.Error.Message);
                }
                return BadRequest(result.Error.Message);
            }
            
            return NoContent();
        }
    }
}