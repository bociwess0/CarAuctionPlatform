using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController] //checking required properties
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<AuctionDTO>>> GetAllAuctions() {
        var auctions = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDTO>>(auctions); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDTO>> GetAuctionId(Guid id) {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if(auction == null) {
            return NotFound();
        }

        return _mapper.Map<AuctionDTO>(auction);

    }

    [HttpPost]
    public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO auctionDTO) {
        var auction = _mapper.Map<Auction>(auctionDTO);

        //add current user as seller

        auction.Seller = "test";
        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) {
            return BadRequest("Could not save changes to database!");
        }

        return CreatedAtAction(nameof(GetAuctionId), new { auction.Id }, _mapper.Map<AuctionDTO>(auction));

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDTO>> UpdateAuction(Guid id, UpdateAuctionDTO updateAuctionDTO) {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) {
            return NotFound();
        }

        //TODO: check seller == username

        auction.Item.Make = updateAuctionDTO.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDTO.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDTO.Color ?? auction.Item.Color;
        auction.Item.Year = updateAuctionDTO.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAuctionDTO.Mileage ?? auction.Item.Mileage;

        var result = await _context.SaveChangesAsync() > 0;

        if(result) {
            return Ok();
        }

        return BadRequest("Could not update the auction!");;

    }

    [HttpDelete("{id}")]

    public async Task<ActionResult<AuctionDTO>> DeleteAuction(Guid id) {

        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) {
            return NotFound();
        }
        
        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if(result) {
            return Ok();
        }

        return BadRequest("Unable to delete auction!");

    }

}
