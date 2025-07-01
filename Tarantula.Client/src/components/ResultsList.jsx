import React from 'react';
import { useQuery } from 'react-query';
import { fetchResults } from '../api/searchApi';

const ResultsList = ({ query }) => {
    const { data, isLoading, error } = useQuery(['results', query], () => fetchResults(query), {
        enabled: !!query
    });

    if (isLoading) return <p className="text-center mt-4">Loading...</p>;
    if (error) return <p className="text-center mt-4 text-red-600">Error fetching results</p>;

    return (
        <div className="mt-6 space-y-4">
            {data.length === 0 && <p className="text-center">No results found.</p>}
            {data.map((item, i) => (
                <div key={i} className="p-4 bg-white rounded shadow-md">
                    <a href={item.url} className="text-blue-600 hover:underline break-all" target="_blank" rel="noreferrer">
                        {item.url}
                    </a>
                    <p className="text-sm text-gray-600 mt-1">{item.meta || item.title}</p>
                    {item.url.match(/\.(jpg|jpeg|png|gif|webp|svg)$/i) && (
                        <img
                            src={item.url}
                            alt={item.meta || 'Image result'}
                            className="mt-2 max-w-full h-auto rounded"
                        />
                    )}
                </div>
            ))}
        </div>
    );
};

export default ResultsList;
