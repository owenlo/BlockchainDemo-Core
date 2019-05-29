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
            if (_context == null)
            {
                _context = context;
            }

            if (_context.BlockItems.Count() == 0)
            {
                int difficulty = GetDifficulty();

                if (difficulty > 0)
                {
                    //Create our genesis block when controller first runs
                    _context.BlockItems.Add(new Block("This is my genesis block", "", difficulty));
                    _context.SaveChanges();
                }
            }
        }

        // GET: api/Blocks
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
        public ActionResult Difficulty()
        {
            return Json("BlockDifficulty:" + GetDifficulty());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Block>>> Simulate()
        {
            string prevHash = _context.BlockItems.Last().Hash;

            Block item = new Block("Some random data " + Guid.NewGuid(), prevHash, GetDifficulty());
            _context.BlockItems.Add(item);

            await _context.SaveChangesAsync();

            return await _context.BlockItems.ToListAsync();
        }

        // POST: api/Blocks
        [HttpPost]
        public async Task<ActionResult<Block>> PostBlock(Block block)
        {
            int currentDifficulty = GetDifficulty();

            if (block.Prevhash != _context.BlockItems.Last().Hash)
            {
                return StatusCode(400);
            }

            if (block.Difficulty != currentDifficulty)
            {
                return StatusCode(400);
            }

            _context.BlockItems.Add(block);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlock), new { id = block.Id }, block);
        }

        private int GetDifficulty()
        {
            int difficulty = int.TryParse(ConfigValueProvider.Get("BlockDifficulty"), out int intDifficulty) ? intDifficulty : 0;

            return difficulty;
        }
    }
}