import { useContext, useState } from "react";
import {  AuthContext  } from './Auth/AuthenticationProvider.tsx'
import {  ToastContainer, toast } from "react-toastify";

 


function Home() {

    const login = useContext(AuthContext);

    const [createList , setCreateList] = useState(false);

    //Create list data

    const [listName , setlistName] = useState("");

    const [listDescription , setlistDescription] = useState("");


    //Handle forms change

    const HandleInputChangeListName = (event : React.ChangeEvent<HTMLInputElement>) => {

        setlistName(event.target.value)

    };


    const HandleInputChangelistDescription = (event : React.ChangeEvent<HTMLInputElement>) => {

        setlistDescription(event.target.value)

    };


    //Hider or display the Create list form

    const ToggleForm = () => {

        return setCreateList(!createList)
    }

    const CreateListAPI = (e: React.FormEvent) =>{

        e.preventDefault();

        const url = 'https://localhost:7237/api/List/CreateList';

        fetch(url , { 
            
            method : 'POST' ,
            
            headers : { 

                'Content-Type' : 'application/json'  

            } ,
            
            body :  JSON.stringify({

                name : listName ,
                description : listDescription ,
                email : login?.getUserEmail()
            })
        
        
        }).then(response => {

            if(response.status === 200){

                return toast.success("List created");

            }

            return toast.error("Error");

             


        });


        
    }

    return ( <>


        
         <h1>Home</h1>

         <ToastContainer />       


        <button onClick={ToggleForm}> Create a List </button>

        { createList && (

            <form onSubmit={CreateListAPI}>

                 
                <input type="text" name="ListName" value={listName} onChange={HandleInputChangeListName} placeholder="List name" />
                <input type="text" name="ListDescription" value={listDescription} onChange={HandleInputChangelistDescription}  placeholder="List Description" />
                <button type="submit" > Create </button>

            </form>

        ) }
    
    </>)


}


export default Home; 