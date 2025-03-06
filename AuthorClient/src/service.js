import axios from 'axios';

const apiUrl = "http://localhost:5261"
axios.defaults.baseURL="http://localhost:5000"

axios.interceptors.response.use(
  response => response, 
  error => {
    console.error("hi"); 

    console.error("API Error:", error.response.status,error.response.data); 
    return Promise.reject(error); 
  }
);
export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/items`)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
      const result = await axios.post(`${apiUrl}/items?Name=${name}`);
      return result.data;
    }
  ,

  setCompleted: async (id, isComplete) => {
    debugger
const result = await axios.put(`${apiUrl}/items/${id}?IsComplete=${isComplete}`);
debugger
    return result.data;
  },
  deleteTask:async(id)=>{
    debugger
    console.log('deleteTask')
    await axios.delete(`${apiUrl}/items/${id}`);
  }
};
