using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using CoreDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [EnableCors("AnotherPolicy")]
    public class EmployeeController : ControllerBase
    {
        private readonly OrionDbContext _context;


        public EmployeeController(OrionDbContext context)
        {
            _context = context;
        }


        [HttpGet(Name = "EmpGet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
        {
            try
            {
                return Ok(await _context.Employees.ToListAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("{code:minlength(5)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetEmployeeByCode(string code)
        {
            try
            {
                if (await this._context.Employees.AnyAsync(e => e.code.ToLower() == code.ToLower()))
                {
                    return Ok(await this._context.Employees.FirstAsync<Employee>(e => e.code.ToLower() == code.ToLower()));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee emp)
        {
            try
            {
                if (!await this._context.Employees.AnyAsync(e => e.code.ToLower() == emp.code.ToLower()))
                {
                    this._context.Employees.Add(emp);
                    await this._context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetEmployeeByCode), new { code = emp.code }, emp);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut("{code:minlength(5)}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(string code, [FromBody] Employee emp)
        {
            try
            {
                if (emp.code.ToLower() != code.ToLower())
                {
                    return BadRequest();
                }
                this._context.Entry(emp).State = EntityState.Modified;

                await this._context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpDelete("{code:minlength(5)}")]
        public async Task<IActionResult> DeleteEmployee(string code)
        {
            try
            {
                if (await this._context.Employees.AnyAsync(e => e.code.ToLower() == code.ToLower()))
                {
                    var emp = await this._context.Employees.FirstAsync(e => e.code.ToLower() == code.ToLower());
                    this._context.Employees.Remove(emp);
                    await this._context.SaveChangesAsync();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
