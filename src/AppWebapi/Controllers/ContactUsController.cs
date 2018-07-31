using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppWebapi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IAsyncRepository<ContactUs> _contactRepository;
        public ContactUsController(IAsyncRepository<ContactUs> contactRepository)
        {
            this._contactRepository = contactRepository;
        }

        /// <summary>
        /// Add a new Contact message
        /// </summary>
        /// <param name="plots"></param>
        /// <returns>A newly create message</returns>
        /// <response code="201">Returns the newly created Contact message</response>
        /// <response code="400">If the Contact message is null</response>           
        [HttpPost]
        [ProducesResponseType(typeof(ContactUs), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]ContactUs contactUs)
        {
            if (contactUs == null)
            {
                return BadRequest();
            }

            await _contactRepository.AddAsync(contactUs);

            return CreatedAtRoute("GetPlot", new { id = contactUs.Id }, contactUs);
        }
    }
}
