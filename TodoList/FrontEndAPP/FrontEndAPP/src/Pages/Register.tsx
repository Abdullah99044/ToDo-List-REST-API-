

import React, { useState } from "react";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";



 
function Register () {


    const [Email , SetEmail] = useState("")

    const [Password , SetPassword] = useState("")

 

    
    const HandleEmail = (event : React.ChangeEvent<HTMLInputElement>) =>{

        SetEmail(event.target.value)
    }   

    const HandlePassowrd = (event : React.ChangeEvent<HTMLInputElement>) => {

        SetPassword(event.target.value)
    }

    const HandleSubmit = (e: React.FormEvent) => {

         e.preventDefault();

        const url = 'https://localhost:7237/register';

        fetch(url ,
            
        { 
            
        method : 'POST' , 

        headers : { 'Content-Type' : 'application/json' } ,

        body : JSON.stringify({

            email : Email ,
            password : Password

        })

        }).then( Res =>  Res.json()

        ).then(data => {
            
            if(data.status != 200){
                
 
                let myUnknown: unknown = Object.values(data.errors)[0];
                let myString: string = myUnknown as string;
               
               
                toast.error(myString[0]);
 
            }
          
        });
    }

    return <>

<ToastContainer />       

 <div className="container   mt-5">
        <div className="row justify-content-center  mt-5">
            <div className="col-lg-4 col-md-6 col-sm-8  mt-5 rounded p-3   "   >
                <div className="card">
                    <div className="card-body bg-black ">
                        <form onSubmit={HandleSubmit}>

                            <div>
                                <label className="form-label mt-2  text-light " >Email : </label>
                                <input  className="form-control"  type="text" name="email" value={Email} placeholder='Email' onChange={HandleEmail}/>
                            </div>

                            <div>
                                <label  className="form-label  mt-2  text-light " >Password : </label>
                                <input className="form-control " type="text" name="Password" value={Password} placeholder='Password' onChange={HandlePassowrd}/>
                            </div>

                            <div className="text-center ">
                                <button   className="btn btn-light mt-4 " type="submit"> submit </button>
                            </div>

                        </form>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </>

}



export default Register;