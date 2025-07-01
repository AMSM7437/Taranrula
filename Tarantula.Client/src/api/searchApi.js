import axios from 'axios';

export const fetchResults = async (query) => {
    const res = await axios.get(`http://localhost:5073/search?query=${encodeURIComponent(query)}`);
    return res.data;
};
