kubectl create namespace m

helm install nginx ingress-nginx/ingress-nginx --namespace m -f https://cdn.otus.ru/media/public/20/14/nginx_ingress-25239-20146a.yaml

kubectl create secret generic secret-appsettings  --namespace m --from-file=./Otus.Microservice.User/appsettings.secrets.json

kubectl apply -f manifest.yml