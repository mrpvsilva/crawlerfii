import axios from "axios";

const API_ENDPOINT = process.env.REACT_APP_API_ENDPOINT;

const client = axios.create({
    baseURL: API_ENDPOINT
});

client.interceptors.request.use(
    config => {

        const value = window.localStorage.getItem("user");

        if (value) {
            const user = JSON.parse(value);
            config.headers['Authorization'] = 'Bearer ' + user?.accessToken
        }

        return config
    },
    error => {
        Promise.reject(error)
    }
)


export default client;