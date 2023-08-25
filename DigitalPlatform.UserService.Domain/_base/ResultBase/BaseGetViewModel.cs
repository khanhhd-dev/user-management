namespace DigitalPlatform.UserService.Domain._base.ResultBase
{
    public abstract class BaseGetViewModel
    {
        public Guid Id { get; set; }
        public string InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual bool ShouldSerializeId()
        {
            return true;
        }

        public virtual bool ShouldSerializeInsertedBy()
        {
            return true;
        }

        public virtual bool ShouldSerializeInsertedAt()
        {
            return true;
        }

        public virtual bool ShouldSerializeUpdatedBy()
        {
            return true;
        }

        public virtual bool ShouldSerializeUpdatedAt()
        {
            return true;
        }
    }

    public abstract class NoAuditBaseGetViewModel : BaseGetViewModel
    {
        public override bool ShouldSerializeId()
        {
            return true;
        }

        public override bool ShouldSerializeInsertedBy()
        {
            return false;
        }

        public override bool ShouldSerializeInsertedAt()
        {
            return false;
        }

        public override bool ShouldSerializeUpdatedBy()
        {
            return false;
        }

        public override bool ShouldSerializeUpdatedAt()
        {
            return false;
        }
    }
}
