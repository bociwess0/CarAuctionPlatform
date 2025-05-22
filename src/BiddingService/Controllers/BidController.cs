using System;
using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/bids")]
public class BidController : ControllerBase
{
    private readonly IMapper _mapper;

    public BidController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDTO>> PlaceBid(string auctionId, int amount) {

        var auction = await DB.Find<Auction>().OneAsync(auctionId);

        System.Console.WriteLine("Auction name: " + auction);

        if(auction == null) {

            System.Console.WriteLine("Auction name: " + auction);

            return NotFound();
        }

        if(auction.Seller == User.Identity.Name) {
            return BadRequest("You cannot bid on your own auction!");
        }

        var bid = new Bid {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
                        .Match(a => a.AuctionId == auctionId)
                        .Sort(b => b.Descending(x => x.Amount))
                        .ExecuteFirstAsync();

            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice
                    ? BidStatus.Accepted
                    : BidStatus.AcceptedBellowReserve;
            }

            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);

        return Ok(_mapper.Map<BidDTO>(bid));

    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDTO>>> GetBidForAuction(string auctionId) {
        var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(a => a.BidTime))
            .ExecuteAsync();

        return bids.Select(_mapper.Map<BidDTO>).ToList();
    }


}
