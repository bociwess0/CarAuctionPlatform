'use client'

import { useParamsStore } from '@/hooks/useParamsStore'
import { usePathname, useRouter } from 'next/navigation'
import React from 'react'
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const router = useRouter();
    const pathname = usePathname();
    const setParams = useParamsStore(state => state.setParams);
    const setSearchValue = useParamsStore(state => state.setSearchvalue)
    const searchValue = useParamsStore(state => state.searchValue);

    function onChange(event: React.ChangeEvent<HTMLInputElement>) {
        setSearchValue(event.target.value);
    }

    function searchHandler() {
        if(pathname !== '/') router.push('/');
        setParams({searchTerm: searchValue})
    }

    return (
        <div className='flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm'>
        <input type="text" placeholder='Search for cars by make, model or color'
            className='input-custom' 
            onChange={onChange}
            onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => {
                if (e.key === 'Enter') searchHandler();
            }}
            value={searchValue}
            />
        <button onClick={searchHandler}>
            <FaSearch size={34} className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'/>
        </button>
        </div>
    )
}
