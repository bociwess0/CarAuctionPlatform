import { getBidForAuction, getDetailedViewData } from '@/app/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React from 'react'
import CountdownTimer from '../../CountdownTimer';
import CardImage from '../../CardImage';
import DetailedSpecs from './DetailedSpecs';
import { getCurrentUser } from '@/app/actions/authActions';
import EditButton from './EditButton';
import DeleteButton from './deleteButton';
import BidItem from './BidItem';
import BidList from './BidList';

export default async function Details({params}: {params: { id: string }}) {
  const { id } = params;
  const data = await getDetailedViewData(params.id);
  const user = await getCurrentUser();
  const bids = await getBidForAuction(id);

  return (
    <div>
      <div className='flex justify-between'>
        <div className='flex items-center gap-3'>
          <Heading title={`${data.make} ${data.model}`} />
          {user?.username === data.seller && (
            <>
              <EditButton id={data.id} />
              <DeleteButton id={data.id} />
            </>
          )}
        </div>
        <div className='flex gap-3'>
          <h3 className='text-2xl font-semibold'>Time remaining:</h3>
          <CountdownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>
      <div className='grid grid-cols-2 gap-6 mt-3'>
        <div className='w-full bg-gray-200 relative aspect-[4/3] aspect-w-16 rounded-lg overflow-hidden'>
          <CardImage imageUrl={data.imageUrl} />
        </div>
        <BidList user={user} auction={data} />
      </div>
      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <DetailedSpecs auction={data} />
      </div>
    </div>
  )
}
