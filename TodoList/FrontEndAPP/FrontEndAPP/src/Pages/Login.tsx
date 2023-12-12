
import {  useState , useContext } from 'react';
import {  AuthContext  } from './Auth/AuthenticationProvider.tsx'
import {  ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate   } from "react-router-dom";


function Login() {


    const  logIn  = useContext(AuthContext);

   
    const navigate = useNavigate();
   

    const [email , setEmail ] = useState("");

    const [password , setPassword ] = useState("");


    const HandleInputChangeEmail = (event : React.ChangeEvent<HTMLInputElement>) => {

        setEmail(event.target.value)

    };


    const HandleInputChangePassword = (event : React.ChangeEvent<HTMLInputElement>) => {

        setPassword(event.target.value)

    };

    

    const HandleSubmit = (e: React.FormEvent) => {

        e.preventDefault();
        

  
 

        fetch('https://localhost:7237/login?useCookies=true&useSessionCookies=true' , 
        { 
            credentials: 'include' ,
            
            method : 'POST' , 
    
            headers : { 
                 'Content-Type' : 'application/json'  
              
        
            } ,
     
           
    
            body : JSON.stringify({
    
                
                email :  email ,
                password:  password,
                twoFactorCode: "string",
                twoFactorRecoveryCode: "string"
             
    
            })
    
            }
        )
        .then(response =>   {


            if(response.status === 200){ 

                logIn?.logIn();
                logIn?.setUserEmail(email);
                navigate('/Home');

            }else{

                
                toast.error("Invalid Login Details : Check your email or password ")

            }

        }).catch(error => console.error(error));
    }


    return <>


        <ToastContainer />       

        <div className="container   mt-5">
        <div className="row justify-content-center  mt-5">
            <div className="col-lg-4 col-md-6 col-sm-8  mt-5 rounded p-3   "   >
                <div className="card">
                    <div className="card-body bg-black ">
                        <form onSubmit={HandleSubmit}>
                            <div className="   mb-3 "   >

                                <div>
                                    <label className="form-label mt-2  text-light " >Email : </label>
                                    <input  className="form-control"  type="text" name="email" value={email} placeholder='Email' onChange={HandleInputChangeEmail} required />
                                </div>

                                <div>
                                    <label  className="form-label  mt-2  text-light " >Password : </label>
                                    <input className="form-control " type="text" name="Password" value={password} placeholder='Password' onChange={HandleInputChangePassword} required />
                                </div>

                                <div className="text-center ">
                                    <button   className="btn btn-light mt-4 " type="submit"> submit </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        </div>

 
    
    </>


}


export default Login; 