# Guids Web Api

<img width="1433" alt="Screen Shot 2024-10-14 at 4 43 16 PM" src="https://github.com/user-attachments/assets/d9cead76-f924-4f47-80f4-ac92a183f55f">


1. Run "docker-compose up --build"
2. Swagger URL : http://localhost:5000/swagger/index.html
3. In order to run Console application navigate to guid.api.client container and start application by typing : "dotnet Guids.Api.Client.dll http://guids.api:5000"

   ```

    $ dotnet Guids.Api.Client.dll http://guids.api:5000

    Base url for Guids.Api: http://guids.api:5000

    List of available commands: 
    ---------------------------------
    help - Display this prompt
    create - Create Guid
    get - Get Guid
    update - Update Guid
    delete - Delete Guid
    quit - Quit the application
    ---------------------------------
    
    Please enter a command or type help for list of commands:
    
    create
    Please enter User.
    Ihar Ihnatovich
    Please enter Expiry Date, specify empty for default.
    2024-10-01T14:18:00.000Z
    Creating Guid...
    Create Guid Result: {
      "guid": "D558C36CE2894CCC8403D805615D077B",
      "user": "Ihar Ihnatovich",
      "expires": "2024-10-01T14:18:00.000Z"
    }
    
```
