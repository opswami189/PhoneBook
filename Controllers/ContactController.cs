using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {

        static List<Contact> Contacts = new List<Contact>();
        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            
            return Ok(Contacts);
        }
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string name, string emailAddress)
        {
            
            
            return Ok(Contacts);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(ContactModel contact)
        {
            if (Contacts.Any(item => string.Equals(item.EmailAddress, contact.EmailAddress, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Email already exists");
            }
            Contact ct = new Contact();
            ct.EmailAddress = contact.EmailAddress;
            ct.Name = contact.Name;
            ct.PhoneNumber = contact.PhoneNumber;
            Contacts.Add(ct);
            return Ok("Success");

        }
        
        [HttpPost]
        [Route("update")]
        public IActionResult Update(Contact contact)
        {
            return Ok("Success");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var itemToRemove = Contacts.SingleOrDefault(r => r.Id == id);
            if (itemToRemove != null)
                Contacts.Remove(itemToRemove);
            return Ok("Deleted");
        }

      
    }
}
