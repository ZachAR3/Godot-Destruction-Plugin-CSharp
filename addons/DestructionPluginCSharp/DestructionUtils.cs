using System.Threading.Tasks;
using Godot;


public partial class DestructionUtils : Node
{
	public async Task<Node3D> CreateShards(Node3D obj, PackedScene shardScene, uint collisionLayers, uint collisionMasks,
		float explosionPower, float fadeDelay, float shrinkDelay)
	{
		Node3D shards = new Node3D();
		shards.Name = obj.Name + "Shards";
		
		await Task.Run(() =>
		{
			foreach (var shardMesh in obj.GetChildren())
			{
				var mesh = shardMesh as MeshInstance3D;
				if (mesh == null)
				{
					continue;
				}

				MeshInstance3D shardMeshTyped = mesh;

				Shard newShard = shardScene.Instantiate<Shard>();
				MeshInstance3D meshInstance = newShard.GetNode<MeshInstance3D>("MeshInstance");
				meshInstance.Mesh = shardMeshTyped.Mesh;

				CollisionShape3D collisionShape = newShard.GetNode<CollisionShape3D>("CollisionShape");
				collisionShape.Shape = meshInstance.Mesh.CreateConvexShape();

				newShard.Position = shardMeshTyped.Position;
				newShard.CollisionLayer = collisionLayers;
				newShard.CollisionMask = collisionMasks;
				newShard.FadeDelay = fadeDelay;
				newShard.ExplosionPower = explosionPower;
				newShard.ShrinkDelay = shrinkDelay;

				
				shards.AddChild(newShard);
			}
			
			obj.QueueFree();
		});
		
		return shards;
	}
			
}