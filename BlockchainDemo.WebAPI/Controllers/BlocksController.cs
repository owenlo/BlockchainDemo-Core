using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemo.Data;
using BlockchainDemo.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlockchainDemo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocksController : Controller
    {
        private readonly BlockContext _context;

        public BlocksController(BlockContext context)
        {
            _context = context;

            if (_context.BlockItems.Count() == 0)
            {
                int difficulty;
                Int32.TryParse(ConfigValueProvider.Get("BlockDifficulty"), out difficulty);

                if (difficulty > 0)
                {
                    //Create our genesis block when controller first runs
                    _context.BlockItems.Add(new Block("This is my genesis block", "", difficulty));
                    _context.SaveChanges();
                }               
            }
        }

        // GET: api/Block
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Block>>> GetBlocks()
        {
            return await _context.BlockItems.ToListAsync();
        }

        // GET: api/Block/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Block>> GetBlock(long id)
        {
            var todoItem = await _context.BlockItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpGet("[action]")] 
        public async Task<ActionResult> Difficulty()
        {
            int difficulty;
            await Task.FromResult(Int32.TryParse(ConfigValueProvider.Get("BlockDifficulty"), out difficulty));

            return Json("BlockDifficulty:" + difficulty);
        }

        // POST: api/Block
        [HttpPost]
        public async Task<ActionResult<Block>> PostBlock(Block item)
        {
            int difficulty;
            Int32.TryParse(ConfigValueProvider.Get("BlockDifficulty"), out difficulty);

            if (item.Prevhash != _context.BlockItems.Last().Hash || item.Difficulty != difficulty)
            {
                return StatusCode(400);
            }

            _context.BlockItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlock), new { id = item.Id }, item);
        }

 
    }
}
