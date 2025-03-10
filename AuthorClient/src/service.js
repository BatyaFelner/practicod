import axios from 'axios';

const apiUrl = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = apiUrl;
console.log(process.env.REACT_APP_API_URL)

axios.interceptors.response.use(
  response => response, 
  error => {
    console.log("nituv:",process.env.REACT_APP_API_URL); 
    console.log(process.env.REACT_APP_API_URL)

    console.error("API Error:", error.response.status,error.response.data); 
    return Promise.reject(error); 
    const result =  axios.get(`${apiUrl}/`)  
    console.log("helo:", result.data); 
  }
);
export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/`)  
    console.log("helo:", result.data); 
  
    return result.data;
  },
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}items`)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
      const result = await axios.post(`${apiUrl}items?Name=${name}`);
      return result.data;
    }
  ,

  setCompleted: async (id, isComplete) => {
    debugger
const result = await axios.put(`${apiUrl}items/${id}?IsComplete=${isComplete}`);
debugger
    return result.data;
  },
  deleteTask:async(id)=>{
    debugger
    console.log('deleteTask')
    await axios.delete(`${apiUrl}items/${id}`);
  }
};
