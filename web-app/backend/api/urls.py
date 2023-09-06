from django.urls import path
from . import views

# URLconf
urlpatterns = [
    path('images', views.ImageList.as_view()),
    path('images/<int:id>', views.ImageItem.as_view())
]