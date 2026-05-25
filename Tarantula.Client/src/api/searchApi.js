import axios from 'axios';

export const fetchResults = async (query) => {
    const res = await axios.get(`/Tarantula/search?query=${encodeURIComponent(query)}`);
    return res.data;
};
