import { useContext, useState , useEffect  } from "react";
import {  AuthContext  } from './Auth/AuthenticationProvider.tsx'
import {  ToastContainer, toast } from "react-toastify";
import { json } from "react-router-dom";

 

interface List {
    id: number;
    name: string;
    description: string;
    createdAt: string;
    updatedAt: string;
  }
  
  interface UserListsResponse {
    lists: List[];
    currentPage : number;
    pages: number;
  }
function Home() {

    const login = useContext(AuthContext);

    const [createList , setCreateList] = useState(false);

    const [ userLists , setUserLists ] = useState<List[]>([]);

    const [ currentPage , setcurrentPage ] = useState(0);

    const [ pages , setpages ] = useState(0);

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


    //Hide or display the Create list form

    const ToggleForm = () => {

        return setCreateList(!createList)
    }

     //Fetch function to get all user lists  from the API
    
    
      useEffect(() => {

 
        const email =  login?.getUserEmail();
        const fetchData = async () => {
          try {
            const response = await fetch('https://localhost:7237/api/v1/Lists/' + email + '?inputPage=3' , {

            method: 'GET',
            credentials: 'include',
        
        }) ;
            if (!response.ok) {
              throw new Error('Failed to fetch user lists');
            }
            const data: UserListsResponse = await response.json();
            setUserLists(data.lists);
            setcurrentPage(data.currentPage);
            setpages(data.pages);
          } catch (error) {
            console.error('Error fetching data:', error);
          }
        };
    
        fetchData();
      }, []);


    //Fetch function to create a list

    const CreateListAPI = (e: React.FormEvent) =>{

        e.preventDefault();

        const url = 'https://localhost:7237/api/v1/Lists';

        fetch(url , { 
            
            credentials: 'include' ,

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

        <ul>
        {userLists.map((item) => (
            <li key={item.id}>

                <h3> {item.name} </h3>
                <p>  {item.description} </p>
            
                
            </li>
        ))}
        </ul>

        <></>
    
    </>)

   
}


export default Home; 