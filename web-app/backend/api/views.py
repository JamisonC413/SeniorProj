import logging
from django.shortcuts import render
from django.http import HttpResponse, JsonResponse, Http404
from rest_framework.views import APIView
from rest_framework import status

from .models import Image
from .serializers import ImageSerializer

import requests

# Create your views here - this is a request handler.
# This code written with the help of Django REST framework tutorial: https://www.django-rest-framework.org/tutorial/

# For interacting with the entire list of images
class ImageList(APIView):
    def post(self, request):
        print('Hit POST /images')

        serializer = ImageSerializer(data=request.data)

        if serializer.is_valid():
            image = serializer.save()
            return JsonResponse(serializer.data, status=status.HTTP_201_CREATED)

        return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    
    def get(self, request):
        print('Hit GET /images')

        images = Image.objects.all()
        serializer = ImageSerializer(images, many=True)

        # safe = False because Django's JsonResponse expects key-value pairs
        return JsonResponse(serializer.data, safe=False)

# For interacting with a specific image
class ImageItem(APIView):
    def get_image(self, id):
        try:
            return Image.objects.get(id=id)
        except Image.DoesNotExist:
            raise Http404

    def get(self, request, id):
        print('Hit POST /images/{id}')

        image = self.get_image(id)
        serializer = ImageSerializer(image)
        return JsonResponse(serializer.data, status=status.HTTP_201_CREATED)

    def delete(self, request, id):
        image = self.get_image(id)
        serializer = ImageSerializer(image)
        image.delete()
        return JsonResponse(serializer.data, status = status.HTTP_204_NO_CONTENT)