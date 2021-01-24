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
        public IActionResult GetAll(int pageNumber,int pageSize=10)
        {

            return Ok(Contacts.Skip(pageSize*(pageNumber-1)).Take(pageSize));
        }
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string name, string emailAddress, int pageNumber, int pageSize=10)
        {
            List<Contact> filteredContacts = Contacts;
            

            if (!string.IsNullOrEmpty(name))
            {
                filteredContacts = Contacts.Where(r => string.Equals(r.Name,name,StringComparison.OrdinalIgnoreCase)).ToList();
              
            }
             if (!string.IsNullOrEmpty(emailAddress))
            {
                filteredContacts = filteredContacts.Where(r => string.Equals(r.EmailAddress,emailAddress,StringComparison.OrdinalIgnoreCase)).ToList();
              
            }
            return Ok(filteredContacts.Skip(pageSize * (pageNumber - 1)).Take(pageSize));
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
        [Route("update/{id}")]
        public IActionResult Update(Guid id, ContactModel contact)
        {
            var itemToUpdate = Contacts.FirstOrDefault(r => r.Id == id);
            if (itemToUpdate == null)
            {
                return BadRequest("Invalid Id");
            }
            if (Contacts.Any(item => item.Id != id && string.Equals(item.EmailAddress, contact.EmailAddress, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Email already associated with other user");
            }

            itemToUpdate.EmailAddress = contact.EmailAddress;
            itemToUpdate.Name = contact.Name;
            itemToUpdate.PhoneNumber = contact.PhoneNumber;
            return Ok("Success");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var itemToRemove = Contacts.FirstOrDefault(r => r.Id == id);
            if (itemToRemove != null)
                Contacts.Remove(itemToRemove);
            return Ok("Deleted");
        }


    }
}
