
import {  useState  } from 'react'


function Login() {

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
        
        fetch( 'https://localhost:7237/login' , 

        {

        method : 'POST' ,
        headers : {   'Content-Type' : 'application/json' } ,
        body : JSON.stringify({

            email : email ,
            password : password

        })}).then(Res => {


            if(Res.status === 200){

                console.log("Good")
            }

            else{

                console.log("Bad")
            }

        }).catch(error => {
            console.error('An error occurred while fetching the data:', error);
        });
    }


    return <>
    
        <div className="container   mt-5">
        <div className="row justify-content-center  mt-5">
            <div className="col-lg-4 col-md-6 col-sm-8  mt-5 rounded p-3   "   >
                <div className="card">
                    <div className="card-body bg-black ">
                        <form onSubmit={HandleSubmit}>
                            <div className="   mb-3 "   >

                                <div>
                                    <label className="form-label mt-2  text-light " >Email : </label>
                                    <input  className="form-control"  type="text" name="email" value={email} placeholder='Email' onChange={HandleInputChangeEmail}/>
                                </div>

                                <div>
                                    <label  className="form-label  mt-2  text-light " >Password : </label>
                                    <input className="form-control " type="text" name="Password" value={password} placeholder='Password' onChange={HandleInputChangePassword}/>
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