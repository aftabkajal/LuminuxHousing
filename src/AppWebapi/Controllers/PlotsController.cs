using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppWebapi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlotsController : ControllerBase
    {
        private readonly IAsyncRepository<Plots> _plotsRepository;

        public PlotsController(IAsyncRepository<Plots> plotsRepository)
        {
            this._plotsRepository = plotsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Plots>> GetAll()
        {
            return await _plotsRepository.ListAllAsync();
        }

        /// <summary>
        /// Get a plots by it's id
        /// </summary>
        /// <param name="id">GUID of a plots</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPlot")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var plot = await _plotsRepository.GetByIdAsync(id);
            if (plot == null)
            {
                return NotFound();
            }

            return new ObjectResult(plot);
        }

        /// <summary>
        /// Add a new plot
        /// </summary>
        /// <param name="plots"></param>
        /// <returns>A newly create plots</returns>
        /// <response code="201">Returns the newly created plots</response>
        /// <response code="400">If the pots is null</response>           
        [HttpPost]
        [ProducesResponseType(typeof(Plots), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]Plots plots)
        {
            if (plots == null)
            {
                return BadRequest();
            }

            await _plotsRepository.AddAsync(plots);

            return CreatedAtRoute("GetPlot", new { id = plots.Id }, plots);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GUID of Plots</param>
        /// <param name="plots"></param>
        /// <returns></returns>
        /// <response code="204">plots was succesfully updated</response>
        /// <response code="400">id or plots is empty</response>
        /// <response code="404">plots not found for the id</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody]Plots plots)
        {
            if (plots == null || plots.Id != id)
            {
                return BadRequest();
            }

            var efPlots = await _plotsRepository.GetByIdAsync(id);
            if (efPlots == null)
            {
                return NotFound();
            }

            efPlots.Name = plots.Name;
            efPlots.OwnerName = plots.OwnerName;
            efPlots.OwnerPhoneNumber = plots.OwnerPhoneNumber;

            await _plotsRepository.UpdateAsync(efPlots);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var plots = await _plotsRepository.GetByIdAsync(id);

            if (plots == null)
            {
                return NotFound();
            }

            await _plotsRepository.DeleteAsync(plots);

            return new NoContentResult();
        }

    }
}