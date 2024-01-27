set "CONNECTION_STRING=Database.SQL"

if NOT "%~1" == "" set "CONNECTION_STRING=%~1"

dotnet ef dbcontext scaffold "Name=%CONNECTION_STRING%" Pomelo.EntityFrameworkCore.MySql -o Models --context-dir Contexts -t landing_banners -t landing_sections -t trips -t trip_details -t trip_detail_images -t trip_images -t trip_schedules -t trip_tags -t users -t trip_recommend -t trip_recommend_item -t user_trips -f --no-onconfiguring