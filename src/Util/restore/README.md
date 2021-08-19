
# Getting Started

Requirements:

- WSL 2
- Docker
- Code with remote containers extension installed
- Docker running with the WSL2 mode enabled [here](https://docs.docker.com/docker-for-windows/wsl/)

Please note: this will probably only work if you pull the code from WSL2 native file system (or a mac), so make sure you git pull from Ubuntu or what ever in to /home somewhere - it will not work from a windows host file system!

Once pulled and the container is running type: `make setup`. 

This will download and restore the database. To test that it has worked, navigate to `script/test.http` and run the URl there (ensure you have hte "REST" extension installed).