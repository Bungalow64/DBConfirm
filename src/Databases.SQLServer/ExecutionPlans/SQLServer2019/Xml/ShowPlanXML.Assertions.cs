namespace DBConfirm.Databases.SQLServer.ExecutionPlans.SQLServer2019.Xml
{
    public partial class ShowPlanXML
	{
	}

	// Other RelOp
	//public partial class SubqueryType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }

	public partial class BaseStmtInfoType
	{
		public virtual QueryPlanType GetQueryPlan() => null;
	}
	public partial class RelOpBaseType
	{
		public virtual RelOpType[] GetRelOp() => new RelOpType[0];
	}
	public partial class RemoteType{ }
	public partial class ConstantScanType {  }
	public partial class RowsetType {  }
	public partial class RemoteQueryType {  }
	public partial class RemoteRangeType { }
	public partial class ScalarInsertType { }
	public partial class UpdateType { }
	public partial class SimpleUpdateType { }
	public partial class IndexScanType { }
	public partial class TableScanType { }

	#region GetQueryPlan
	public partial class StmtSimpleType { public override QueryPlanType GetQueryPlan() => QueryPlan; }
	public partial class StmtCondType { public override QueryPlanType GetQueryPlan() => null; }
	public partial class StmtReceiveType { public override QueryPlanType GetQueryPlan() => null; }
	public partial class StmtCursorType { public override QueryPlanType GetQueryPlan() => null; }
	public partial class StmtUseDbType { public override QueryPlanType GetQueryPlan() => null; }
	#endregion

	#region GetRelOp

	public partial class AdaptiveJoinType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class GetType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class DMLOpType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class LocalCubeType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class GbAggType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class GbApplyType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class JoinType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class ProjectType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class ExternalSelectType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class MoveType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class GenericType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class PutType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class RemoteModifyType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class RemoteFetchType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class BatchHashTableBuildType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class SpoolType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class WindowAggregateType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class WindowType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class UDXType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class TopType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class SplitType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class SequenceType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class SegmentType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class NestedLoopsType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class MergeType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class ConcatType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class CollapseType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class BitmapType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class SortType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class StreamAggregateType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class ParallelismType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class ComputeScalarType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class HashType { public override RelOpType[] GetRelOp() => RelOp; }
	public partial class TableValuedFunctionType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class FilterType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class SimpleIteratorOneChildType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class CreateIndexType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class XcsScanType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }
	public partial class ForeignKeyReferencesCheckType { public override RelOpType[] GetRelOp() => new RelOpType[] { RelOp }; }

	#endregion
}
