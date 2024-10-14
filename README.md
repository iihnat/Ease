<img width="1428" alt="Screen Shot 2024-10-14 at 4 40 29 PM" src="https://github.com/user-attachments/assets/3b4eda55-ed18-4f12-9396-67ce6760e738"># Ease
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
<img width="1431" alt="Screen Shot 2024-10-14 at 4 40 58 PM" src="https://github.com/user-attachments/assets/b159dcd2-2a21-4b72-a657-7b98b5135e64">

