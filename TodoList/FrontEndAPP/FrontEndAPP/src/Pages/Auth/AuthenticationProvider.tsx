import { useState , createContext , ReactNode } from 'react';
 



interface Authentication {
    authenticated: boolean;
    logIn: () => void;
    SetJwtKey: (Key : string  ) => void;
    ReturnJwt: () => string;
    logout: () => void;

}

//Authentication context

const AuthContext = createContext<Authentication | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

//Authentiction provider to wrap around routers

const AuthProvider = ({ children }: AuthProviderProps) => {


    const [authenticated, setAuthenticated] = useState(!!sessionStorage.getItem('Islogged'));

    const [JWT , setJWT] = useState("");


    const SetJwtKey = (Key : string  ) => {

      setJWT(Key)
  
    };

    const ReturnJwt = () => {

      return JWT;
    }
  
    const logIn = () => {

      sessionStorage.setItem('Islogged', "true" );
      setAuthenticated(true);
    };
  
    const logout = () => {
      // Perform logout logic

      sessionStorage.removeItem('Islogged');
      setAuthenticated(false);
         
      
    };
  
    return (
      <AuthContext.Provider value={{ authenticated   , logIn , SetJwtKey , ReturnJwt , logout }}>
        {children}
      </AuthContext.Provider>
    );
  };
  
  export { AuthContext, AuthProvider };
 
   




 
