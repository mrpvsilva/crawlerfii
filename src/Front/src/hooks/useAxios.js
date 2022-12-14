import { useState, useEffect } from 'react';
import api from '../Api'

const useAxios = ({ url, method = 'get', body = null, headers = null }) => {

    const [response, setResponse] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {

        const fetch = async () => {
            try {
                const res = await api[method](url, JSON.parse(headers), JSON.parse(body));
                setResponse(res.data);
            } catch (err) {
                setError(err);
            } finally {
                setLoading(false);
            }
        };

        fetch();
    }, [method, url, body, headers]);

    return { response, error, loading };
};

export default useAxios;