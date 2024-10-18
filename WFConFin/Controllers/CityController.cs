using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : Controller
    {
        private readonly WFConFinDbContext _context;

        public CityController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region Get City
        [HttpGet]
        public IActionResult GetCity()
        {
            try
            {
                var city = _context.City.Include(c => c.State).OrderBy(c => c.Name).ToList();
                return Ok(city);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region GetCityId
        [HttpGet("{id}")]
        public IActionResult GetCity([FromRoute] Guid id)
        {
            try
            {
                City city = _context.City.Find(id) ?? throw new NullReferenceException("City Not found");

                return Ok(city);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSiglaPagination
        [HttpGet("Pagination")]
        public IActionResult GetCityPagination([FromQuery] string value, int skip, int take, bool desc)
        {
            try
            {
                value = value.ToUpper();

                var city = from o in _context.City.ToList()
                            where o.Name.ToUpper().Contains(value) || o.StateSigla.ToUpper().Contains(value)
                            select o;

                _ = city.FirstOrDefault() ?? throw new NullReferenceException("City not found");

                if (desc)
                {
                    city = from o in city orderby o.Name descending select o;
                }
                else
                {
                    city = from o in city orderby o.Name ascending select o;
                }

                int amount = city.Count();

                city = city.Skip(skip).Take(take).ToList();

                var pr = new PaginationResponse<City>(city, amount, skip, take);

                return Ok(pr);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetStatePagination With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSiglaSmart
        [HttpGet("Search")]
        public IActionResult GetSiglaSeach([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();

                var city = _context.City.Where(c => c.Name.ToUpper().Contains(value) || c.StateSigla.ToUpper().Contains(value)).ToList();

                _ = city.FirstOrDefault() ?? throw new NullReferenceException($"Estado não existe no banco de dados");

                return Ok(city);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSiglaSearch With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Post City
        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {
            try
            {
                _context.City.Add(city);
                var value = _context.SaveChanges();

                if (value == 1)
                {
                    return Ok("The city was successfully registered");
                }
                else
                {
                    return BadRequest("Problem: City don't registered");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region Put City
        [HttpPut]
        public IActionResult PutCity([FromBody] City city)
        {
            try
            {
                City obj = _context.City.Find(city.Id) ?? throw new NullReferenceException("City not found");

                _context.City.Update(city);
                int value = _context.SaveChanges();

                if (value == 1)
                {
                    return Ok("The city was Updated");
                }
                else
                {
                    return BadRequest("Problem: City don't Updated");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region DeleteCity
        [HttpDelete("{id}")]
        public IActionResult DeleteCity([FromRoute] Guid id)
        {
            try
            {
                City city = _context.City.Find(id) ?? throw new NullReferenceException("City not Found");

                _context.City.Remove(city);

                int value = _context.SaveChanges();

                if (value == 1)
                {
                    return Ok("The city was Deleted");
                }
                else
                {
                    return BadRequest("Problem: City don't Deleted");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

    }
}
